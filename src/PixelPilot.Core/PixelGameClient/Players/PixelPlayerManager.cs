using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.Players;

/// <summary>
/// Class that handles players in a world.
/// Automatically updates the players stats.
/// </summary>
public abstract class PixelPlayerManager<T> where T : IPixelPlayer
{
    private ILogger _logger = LogManager.GetLogger("WorldPlayers");
    
    private Dictionary<int, T> _players = new();
    public IEnumerable<T> Players => _players.Values;
    public int CrownedPlayerId
    {
        get;
        private set;
    } = -1;
    public int ClientId { get; private set; }
    
    /// <summary>
    /// Fired before the player properties are changed.
    /// </summary>
    public event PrePlayerStatusChange? OnPrePlayerStatusChange;
    public delegate void PrePlayerStatusChange(object sender, T player, IPixelGamePlayerPacket packet);

    /// <summary>
    /// Fired once the players properties have been updated.
    /// </summary>
    public event PlayerStatusChanged? OnPlayerStatusChanged;
    public delegate void PlayerStatusChanged(object sender, T player);
    
    /// <summary>
    /// Fired when a player leaves the world.
    /// </summary>
    public event PlayerLeft? OnPlayerLeft;
    public delegate void PlayerLeft(object sender, T player);
    
    
    public T? CrownedPlayer => GetPlayer(CrownedPlayerId);

    /// <summary>
    /// Get the player based on it's ID. Returns NULL if no player was found.
    /// </summary>
    /// <param name="id">The player ID</param>
    /// <returns>A IPixelPlayer or NULL</returns>
    public T? GetPlayer(int id)
    {
        return _players.GetValueOrDefault(id);
    }

    /// <summary>
    /// Used to create a new instance of IPixelPlayer
    /// </summary>
    /// <param name="join">The join packet</param>
    /// <returns>A new IPixelPlayer instance</returns>
    protected abstract T CreatePlayer(PlayerJoinPacket join);
    
    /// <summary>
    /// Method that can receive packets and handle them accordingly.
    /// Should update the player state and fire relevant events.
    /// </summary>
    /// <param name="sender">The packet sender</param>
    /// <param name="packet">The packet</param>
    public void HandlePacket(object sender, IPixelGamePacket packet)
    {
        if (packet is InitPacket init)
        {
            ClientId = init.PlayerId;
            return;
        }
        if (packet is not IPixelGamePlayerPacket playerPacket) return;

        // Join check should always be done first.
        // Seperated out for clarity.
        if (playerPacket is PlayerJoinPacket join)
        {
            _players[join.PlayerId] = CreatePlayer(join);
            _logger.LogDebug($"'{join.Username}' joined the world.");
            return;
        }

        // Player left the world.
        if (playerPacket is PlayerLeftPacket left)
        {
            var playerFound = _players.Remove(left.PlayerId, out var leftPlayer);
            if (!playerFound) return;
            
            OnPlayerLeft?.Invoke(this, leftPlayer!);
            _logger.LogDebug($"'{leftPlayer!.Username}' left the world.");
            return;
        }

        // Don't track the for now.
        if (playerPacket.PlayerId == ClientId) return;

        if (!_players.ContainsKey(playerPacket.PlayerId))
        {
            _logger.LogDebug($"{packet.GetType().Name} received but player {playerPacket.PlayerId} does not exist.");
            return;
        }
        
        // Player is always present now.
        var player = _players[playerPacket.PlayerId];
        OnPrePlayerStatusChange?.Invoke(this, player, playerPacket);
        
        // Update player state based on the received message.
        switch (packet)
        {
            case PlayerModMode mod:
                player.Modmode = mod.IsEnabled;
                break;
            case PlayerGodmodePacket god:
                player.Godmode = god.IsEnabled;
                break;
            case PlayerMovePacket move:
                player.X = move.X;
                player.Y = move.Y;
                player.ModX = move.ModX;
                player.ModY = move.ModY;
                player.VelocityX = move.VelocityX;
                player.VelocityY = move.VelocityY;
                player.Horizontal = move.Horizontal;
                player.Vertical = move.Vertical;
                player.Spacedown = move.Spacedown;
                player.SpaceJustDown = move.SpaceJustDown;
                player.TickId = move.TickId;
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.Crown}:
                // Remove crown from other player.
                if (CrownedPlayerId != -1 && _players.TryGetValue(CrownedPlayerId, out var otherPlayer)) otherPlayer.HasCrown = false;
                CrownedPlayerId = player.Id;
                player.HasCrown = true;
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.Trophy}:
                player.HasCompletedWorld = true;
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.ResetPoint} block:
                ResetPlayer(player, block.X, block.Y);
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.GodModeActivator}:
                player.CanGod = true;
                break;
            case PlayerResetPacket reset:
                ResetPlayer(player, reset.X, reset.Y);
                break;
            case PlayerRespawnPacket respawn:
                player.X = respawn.X;
                player.Y = respawn.Y;
                break;
            case PlayerStatsChangePacket stats:
                player.GoldCoins = stats.GoldCoins;
                player.BlueCoins = stats.BlueCoins;
                player.Deaths = stats.DeathCount;
                break;
            case PlayerUpdateRightsPacket rights:
                player.CanEdit = rights.EditRights;
                player.CanGod = rights.Godmode;
                break;
            default:
                _logger.LogDebug($"Unhandled PlayerPacket {playerPacket.GetType().Namespace}");
                break;
        }
        
        OnPlayerStatusChanged?.Invoke(sender, player);
    }

    private void ResetPlayer(T player, int x, int y)
    {
        if (CrownedPlayerId == player.Id) CrownedPlayerId = -1;
        
        player.HasCompletedWorld = false;
        player.HasCrown = false;
        player.X = x;
        player.Y = y;
    }
}