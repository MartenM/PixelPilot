using System.Drawing;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelPilot.Common.Logging;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Players;

/// <summary>
/// Class that handles players in a world.
/// Automatically updates the players stats.
/// </summary>
public abstract class PixelPlayerManager<T> where T : IPixelPlayer
{
    protected ILogger _logger = LogManager.GetLogger("WorldPlayers");
    
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
    public delegate void PrePlayerStatusChange(object sender, T player, IMessage packet);

    /// <summary>
    /// Fired once the players properties have been updated.
    /// </summary>
    public event PlayerStatusChanged? OnPlayerStatusChanged;
    public delegate void PlayerStatusChanged(object sender, T player);
    
    /// <summary>
    /// Fired when a player leaves the world.
    /// </summary>
    public event PlayerJoined? OnPlayerJoined;
    public delegate void PlayerJoined(object sender, T player);
    
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
    /// Gets the player by username (case invariant). Returns NULL if no player was found.
    /// </summary>
    /// <param name="username">The username</param>
    /// <returns></returns>
    public T? GetPlayerByUsername(string username)
    {
        return _players.Values.FirstOrDefault(p =>
            string.Equals(p.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Used to create a new instance of IPixelPlayer
    /// </summary>
    /// <param name="join">The join packet</param>
    /// <returns>A new IPixelPlayer instance</returns>
    protected abstract T CreatePlayer(PlayerJoinedPacket join);
    
    /// <summary>
    /// Method that can receive packets and handle them accordingly.
    /// Should update the player state and fire relevant events.
    /// </summary>
    /// <param name="sender">The packet sender</param>
    /// <param name="packet">The packet</param>
    public void HandlePacket(object sender, IMessage packet)
    {
        if (packet is PlayerInitPacket init)
        {
            ClientId = init.PlayerProperties.PlayerId;
            return;
        }

        // Join check should always be done first.
        // Seperated out for clarity.
        if (packet is PlayerJoinedPacket joinedPacket)
        {
            _players[joinedPacket.Properties.PlayerId] = CreatePlayer(joinedPacket);
            _logger.LogDebug($"'{joinedPacket.Properties.Username}' joined the world.");
            OnPlayerJoined?.Invoke(this, _players[joinedPacket.Properties.PlayerId]);
            return;
        }

        // Player left the world.
        if (packet is PlayerLeftPacket left)
        {
            var playerFound = _players.Remove(left.PlayerId, out var leftPlayer);
            if (!playerFound) return;
            
            OnPlayerLeft?.Invoke(this, leftPlayer!);
            _logger.LogDebug($"'{leftPlayer!.Username}' left the world.");
            return;
        }

        // Don't track the for now.
        var optionalPlayerId = packet.GetPlayerId();
        if (optionalPlayerId == null || optionalPlayerId == ClientId) return;
        
        var playerId = optionalPlayerId.Value;

        if (!_players.ContainsKey(playerId))
        {
            _logger.LogDebug($"{packet.GetType().Name} received but player {playerId} does not exist.");
            return;
        }
        
        // Player is always present now.
        var player = _players[playerId];
        OnPrePlayerStatusChange?.Invoke(this, player, packet);
        
        // Update player state based on the received message.
        switch (packet)
        {
            case PlayerModModePacket mod:
                player.Modmode = mod.Enabled;
                break;
            case PlayerGodModePacket god:
                player.Godmode = god.Enabled;
                break;
            case PlayerMovedPacket move:
                player.X = move.Position.X;
                player.Y = move.Position.Y;
                player.ModX = move.ModifierX;
                player.ModY = move.ModifierY;
                player.VelocityX = move.VelocityX;
                player.VelocityY = move.VelocityY;
                player.Horizontal = move.Horizontal;
                player.Vertical = move.Vertical;
                player.Spacedown = move.SpaceDown;
                player.SpaceJustDown = move.SpaceJustDown;
                player.TickId = move.TickId;
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.CrownGold}:
                // Remove crown from other player.
                if (CrownedPlayerId != -1 && _players.TryGetValue(CrownedPlayerId, out var otherPlayer)) otherPlayer.HasCrown = false;
                CrownedPlayerId = player.Id;
                player.HasCrown = true;
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.CrownSilver}:
                player.HasCompletedWorld = true;
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.ToolReset} block:
                ResetPlayer(player, block.Position.ToPoint());
                break;
            case PlayerTouchBlockPacket {BlockId: (int) PixelBlock.ToolGodModeActivator}:
                player.CanGod = true;
                break;
            case PlayerResetPacket reset:
                ResetPlayer(player, reset.Position?.ToPoint() ?? null);
                break;
            case PlayerRespawnPacket respawn:
                player.X = respawn.Position.X;
                player.Y = respawn.Position.Y;
                break;
            case PlayerCountersUpdatePacket counters:
                player.GoldCoins = counters.Coins;
                player.BlueCoins = counters.BlueCoins;
                player.Deaths = counters.Deaths;
                break;
            case PlayerUpdateRightsPacket rights:
                player.CanEdit = rights.Rights.CanEdit;
                player.CanGod = rights.Rights.CanGod;
                break;
            default:
                _logger.LogDebug($"Unhandled PlayerPacket {packet.GetType().Namespace}");
                break;
        }
        
        OnPlayerStatusChanged?.Invoke(sender, player);
    }

    private void ResetPlayer(T player, Point? position)
    {
        if (CrownedPlayerId == player.Id) CrownedPlayerId = -1;
        
        player.HasCompletedWorld = false;
        player.HasCrown = false;
        if (position != null)
        {
            player.X = position.Value.X;
            player.Y = position.Value.Y;
        }
        
    }
}