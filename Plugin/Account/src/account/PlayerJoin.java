package account;

import org.bukkit.entity.Player;
import org.bukkit.event.player.PlayerJoinEvent;
import org.bukkit.event.player.PlayerListener;

public class PlayerJoin extends PlayerListener
{
    private Main plugin;
    
    public PlayerJoin(Main plugin)
    {
        this.plugin = plugin;
    }
    
    @Override
    public void onPlayerJoin(PlayerJoinEvent event)
    {
        Player player = event.getPlayer();
        
        String address = player.getAddress().getHostString();
        
        if (!Main.VerifyLoginRequest(address, player.getName()))
        {
            player.kickPlayer("Please log in.");
        }
    }
}
