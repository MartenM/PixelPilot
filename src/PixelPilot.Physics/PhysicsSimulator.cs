using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Players;
using PixelPilot.PixelGameClient.Players.Basic;
using PixelPilot.PixelGameClient.World;
using PixelPilot.PixelGameClient.World.Blocks;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.Physics;

public class PhysicsSimulator
{
    private PixelPilotClient _client;
    private PixelWorld _world;

    private PhysicsConfig _physicsConfig = new PhysicsConfig();
    
    private Dictionary<int, PhysicsPlayer> _players = new();

    private Timer? _timer;

    public void AddPlayer(IPixelPlayer player)
    {
        _players.Add(player.Id, new PhysicsPlayer(player));
    }

    public void RemovePlayer(int id)
    {
        _players.Remove(id);
    }

    public void TickPhysics()
    {
        foreach (var player in _players.Values)
        {
            player.FromMirror();
            TickPlayer(player);
            player.ToMirror();
        }
    }

    public void StartSimulation()
    {
        _timer = new Timer((state =>
        {
            TickPhysics();
        }), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_physicsConfig.MsPerTick));
    }

    public void TickPlayer(PhysicsPlayer player)
    {
        if (player.IsDead) return;

        PixelBlock? delayed = player.Queue.Count > 0 ? player.Queue.Dequeue() : null;
        var cx = ((int) player.X + 8) >> 4;
        var cy = ((int) player.Y + 8) >> 4;

        var currentBlockData = _world.BlockAt(WorldLayer.Foreground, cx, cy);
        var currentBlock = currentBlockData.Block;
        player.Queue.Enqueue(currentBlock);

        if (currentBlock == PixelBlock.GravityDot || currentBlock.IsClimbable())
        {
            delayed = player.Queue.Dequeue();
            player.Queue.Enqueue(currentBlock);
        }

        PixelBlock? delayedBlock = delayed != null ? currentBlock : null;
        
        player.MorX = 0;
        player.MorY = 0;
        player.MoX = 0;
        player.MoY = 0;

        if (!player.IsFlying)
        {
            if (currentBlock.IsClimbable() || currentBlock.IsBoost())
            {
                player.MorX = 0;
                player.MorY = 0;
            }
            else
            {
                switch (currentBlock)
                {
                    case PixelBlock.GravityLeft:
                        player.MorX = - _physicsConfig.Gravity;
                        player.MorY = 0;
                        break;
                    case PixelBlock.GravityUp:
                        player.MorX = 0;
                        player.MorY = - _physicsConfig.Gravity;;
                        break;
                    case PixelBlock.GravityRight:
                        player.MorX = _physicsConfig.Gravity;;
                        player.MorY = 0;
                        break;
                    case PixelBlock.GravityDown:
                        player.MorX = 0;
                        player.MorY = _physicsConfig.Gravity;;
                        break;
                    case PixelBlock.GravityDot:
                        player.MorX = 0;
                        player.MorY = 0;
                        break;
                    case PixelBlock.Water:
                        player.MorX = 0;
                        player.MorY = -1;
                        break;
                    default:
                        player.MorX = 0;
                        player.MorY = _physicsConfig.Gravity;;
                        break;
                }
            }

            if (delayedBlock?.IsClimbable() == true || delayedBlock?.IsBoost() == true)
            {
                player.MoX = 0;
                player.MoY = 0;
            }
            else
            {
                switch (delayed)
                {
                    case PixelBlock.GravityLeft:
                        player.MoX = - _physicsConfig.Gravity;
                        player.MoY = 0;
                        break;
                    case PixelBlock.GravityUp:
                        player.MoX = 0;
                        player.MoY = -_physicsConfig.Gravity;
                        break;
                    case PixelBlock.GravityRight:
                        player.MoX = _physicsConfig.Gravity;
                        player.MoY = 0;
                        break;
                    case PixelBlock.GravityDown:
                        player.MoX = 0;
                        player.MoY = _physicsConfig.Gravity;
                        break;
                    case PixelBlock.GravityDot:
                        player.MoX = 0;
                        player.MoY = 0;
                        break;
                    case PixelBlock.Water:
                        player.MoX = 0;
                        player.MoY = _physicsConfig.Buoyancy.Water;
                        break;
                    default:
                        player.MoX = 0;
                        player.MoY = _physicsConfig.Gravity;
                        break;
                }
            }
        }

        if (delayedBlock?.IsLiquide() ?? false)
        {
            player.Mx = player.Horizontal;
            player.My = player.Vertical;
        }
        else if (player.MoY != 0)
        {
            player.Mx = player.Horizontal;
            player.My = 0;
        }
        else if (player.MoX != 0)
        {
            player.Mx = 0;
            player.My = player.Vertical;
        }
        else
        {
            player.Mx = player.Horizontal;
            player.My = player.Vertical;
        }

        player.ModifierX = player.MoX + player.Mx;
        player.ModifierY = player.MoY + player.My;

        if (player.TempSpeedX != 0 || player.TempModifierX != 0)
        {
            player.TempSpeedX += player.TempModifierX;
            if (!player.IsFlying)
            {
                if (player.Mx == 0 && player.MoY != 0 || player.TempSpeedX < 0 && player.Mx > 0 ||
                    player.TempSpeedX > 0 && player.Mx < 0)
                {
                    player.TempSpeedX *= _physicsConfig.Drag.Base;
                    player.TempSpeedX *= _physicsConfig.Drag.NoModifier;
                }
                else if (currentBlock.IsClimbable())
                {
                    player.TempSpeedX *= _physicsConfig.Drag.Base;
                    player.TempSpeedX *= _physicsConfig.Drag.Ladder;
                }
                else if (currentBlock == PixelBlock.Water)
                {
                    player.TempSpeedX *= _physicsConfig.Drag.Base;
                    player.TempSpeedX *= _physicsConfig.Drag.Water;
                }
                else
                {
                    player.TempSpeedX *= _physicsConfig.Drag.Base;
                }
            }
            else
            {
                player.TempSpeedX *= _physicsConfig.Drag.Base;
            }

            player.TempSpeedX = Math.Clamp(player.TempSpeedX, -16.0f, 16.0f);
            player.TempSpeedX = Math.Abs(player.TempSpeedX) < 0.0001f ? 0 : player.TempSpeedX;
        }

        if (player.TempSpeedY != 0 || player.TempModifierY != 0)
        {
            player.TempSpeedY += player.TempModifierY;
            if (!player.IsFlying)
            {
                if (player.My == 0 && player.MoX != 0 || player.TempSpeedY < 0 && player.My > 0 ||
                    player.TempSpeedY > 0 && player.My < 0)
                {
                    player.TempSpeedY *= _physicsConfig.Drag.Base;
                    player.TempSpeedY *= _physicsConfig.Drag.NoModifier;
                }
                else if (currentBlock.IsClimbable())
                {
                    player.TempSpeedY *= _physicsConfig.Drag.Base;
                    player.TempSpeedY *= _physicsConfig.Drag.Ladder;
                }
                else if (currentBlock == PixelBlock.Water)
                {
                    player.TempSpeedY *= _physicsConfig.Drag.Base;
                    player.TempSpeedY *= _physicsConfig.Drag.Water;
                }
                else
                {
                    player.TempSpeedY *= _physicsConfig.Drag.Base;
                }
            }
            else
            {
                player.TempSpeedY *= _physicsConfig.Drag.Base;
            }

            player.TempSpeedY = Math.Clamp(player.TempSpeedY, -16.0f, 16.0f);
            player.TempSpeedY = Math.Abs(player.TempSpeedY) < 0.0001f ? 0 : player.TempSpeedY;
        }

        if (!player.IsFlying && currentBlock.IsBoost())
        {
            // Not sure what this means
            switch (currentBlock)
            {
                case PixelBlock.BoostLeft:
                    player.TempSpeedX = _physicsConfig.BoostSpeed * -1;
                    break;
                case PixelBlock.BoostRight:
                    player.TempSpeedX = _physicsConfig.BoostSpeed * 1;
                    break;
                case PixelBlock.BoostDown:
                    player.TempSpeedY = _physicsConfig.BoostSpeed * 1;
                    break;
                case PixelBlock.BoostUp:
                    player.TempSpeedY = _physicsConfig.BoostSpeed * -1;
                    break;
            }
        }

        player.ReminderX = player.X % 1;
        player.CurrentSpeedX = player.TempSpeedX;
        player.ReminderY = player.Y % 1;
        player.CurrentSpeedY = player.TempSpeedY;

        bool doneX = false;
        bool doneY = false;
        bool grounded = false;
        player.JustTeleported = false;

        while ((player.CurrentSpeedX != 0 && !doneX) || (player.CurrentSpeedY != 0 && !doneY))
        {
            if (player.Id == _client.BotId)
            {
                // player.ProcessPortals(currentBlockData, cx, cy);
            }

            player.Ox = player.X;
            player.Oy = player.Y;
            var osx = player.CurrentSpeedX;
            var osy = player.CurrentSpeedY;

            if (player.CurrentSpeedX > 0)
            {
                if (player.CurrentSpeedX + player.ReminderX >= 1)
                {
                    player.X +=  (1 - player.ReminderX);
                    // player.X >>= 0;
                    player.CurrentSpeedX -= (1 - player.ReminderX);
                    player.ReminderX = 0;
                }
                else
                {
                    player.X += player.CurrentSpeedX;
                    player.CurrentSpeedX = 0;
                }
            }
            else if (player.CurrentSpeedX < 0)
            {
                if (player.ReminderX + player.CurrentSpeedX < 0 && (player.ReminderX != 0 || currentBlock.IsBoost()))
                {
                    player.CurrentSpeedX += player.ReminderX;
                    player.X -= player.ReminderX;
                    player.X = (int) player.X;
                    player.ReminderX = 1;
                }
                else
                {
                    player.X += player.CurrentSpeedX;
                    player.CurrentSpeedX = 0;
                }
            }

            // Collision checks with world.
            if (player.state.world.CollidesWith(player))
            {
                player.X = player.Ox;
                if (player.TempSpeedX > 0 && player.MorX > 0)
                {
                    grounded = true;
                }

                if (player.TempSpeedX < 0 && player.MorX < 0)
                {
                    grounded = true;
                }

                player.TempSpeedX = 0;
                player.CurrentSpeedX = osx;
                doneX = true;
            }

            if (player.CurrentSpeedY > 0)
            {
                if (player.CurrentSpeedY + player.ReminderY >= 1)
                {
                    player.Y += (1 - player.ReminderY);
                    player.Y = (int)player.Y;
                    player.CurrentSpeedY -= (1 - player.ReminderY);
                    player.ReminderY = 0;
                }
                else
                {
                    player.Y += player.CurrentSpeedY;
                    player.CurrentSpeedY = 0;
                }
            }
            else if (player.CurrentSpeedY < 0)
            {
                if (player.ReminderY + player.CurrentSpeedY < 0 && (player.ReminderY != 0 || currentBlock.IsBoost()))
                {
                    player.Y -= player.ReminderY;
                    player.Y = (int)player.Y;
                    player.CurrentSpeedY += player.ReminderY;
                    player.ReminderY = 1;
                }
                else
                {
                    player.Y += player.CurrentSpeedY;
                    player.CurrentSpeedY = 0;
                }
            }

            if (player.state.world.CollidesWith(player))
            {
                player.Y = player.Oy;
                if (player.TempSpeedY > 0 && player.MorY > 0)
                {
                    grounded = true;
                }

                if (player.TempSpeedY < 0 && player.MorY < 0)
                {
                    grounded = true;
                }

                player.TempSpeedY = 0;
                player.CurrentSpeedY = osy;
                doneY = true;
            }
        }

        bool inJump = false;
        int jumpMod = 1;
        long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        bool inLiquid = currentBlock.IsLiquide() && !player.IsFlying;

        if (player.SpaceJustDown && !inLiquid)
        {
            player.LastJump = -currentTime;
            inJump = true;
            jumpMod = -1;
        }

        if (player.SpaceDown && !inLiquid)
        {
            if (player.LastJump < 0)
            {
                if (currentTime + player.LastJump > 750)
                {
                    inJump = true;
                }
            }
            else
            {
                if (currentTime - player.LastJump > 150)
                {
                    inJump = true;
                }
            }
        }

        if (inJump)
        {
            if (player.TempSpeedX == 0 && player.MorX != 0 && player.MoX != 0 && grounded)
            {
                player.TempSpeedX -= player.MorX * _physicsConfig.JumpHeight;
                player.LastJump = currentTime * jumpMod;
            }

            if (player.TempSpeedY == 0 && player.MorY != 0 && player.MoY != 0 && grounded)
            {
                player.TempSpeedY -= player.MorY * _physicsConfig.JumpHeight;
                player.LastJump = currentTime * jumpMod;
            }
        }

        var imx = (int) player.TempSpeedX << 8;
        var imy = (int) player.TempSpeedY << 8;
        bool moving = imx != 0 || imy != 0;

        if (!moving && !inLiquid)
        {
            player.X = SnapToGrid(player.X, player.TempModifierX);
            player.Y = SnapToGrid(player.Y, player.TempModifierY);
        }
    }

    private double SnapToGrid(double axis, double modifier)
    {
        if (modifier < 0.1 && modifier > -0.1)
        {
            double tx = axis % 16;
            if (tx < 2)
            {
                if (tx < 0.2)
                {
                    axis = (int)axis;
                }
                else
                {
                    axis -= tx / 15;
                }
            }
            else if (tx > 14)
            {
                if (tx > 15.8)
                {
                    axis = (int)axis;
                    axis++;
                }
                else
                {
                    axis += (tx - 14) / 15;
                }
            }
        }
        return axis;
    }
}