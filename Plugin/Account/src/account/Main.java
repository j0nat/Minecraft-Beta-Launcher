package account;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import org.bukkit.event.Event;
import org.bukkit.event.Event.Priority;
import org.bukkit.plugin.PluginManager;
import org.bukkit.plugin.java.*;

public class Main extends JavaPlugin
{
        // ACCOUNT PLUGIN VARIABLES -------------------------------------------
        public static final int PORT = 1521; // Server port number
        public static final int LOGIN_TIMEOUT = 120; // Two minute timeout
        public static volatile String ACCOUNTS_FILE = "accounts.txt"; // File to store accounts to as JSONArray
        public static volatile boolean IS_RUNNING = true;
        public static volatile ArrayList<LoginRequest> LOGIN_REQUESTS = new ArrayList<LoginRequest>();
        // --------------------------------------------------------------------
        
        private Server server;
        private AccountManager accountManager;
    
	@Override
	public void onEnable()
	{
            ACCOUNTS_FILE = new File(this.getDataFolder().getAbsolutePath() + File.separator + ACCOUNTS_FILE).getAbsolutePath();
            
            File pluginFolder = new File(this.getDataFolder() + "/");
            if(!pluginFolder.exists())
            {
                pluginFolder.mkdir();
            }
            
            PlayerJoin playerJoin = new PlayerJoin(this);
            PluginManager pluginManager = getServer().getPluginManager();
            pluginManager.registerEvent(Event.Type.PLAYER_JOIN, playerJoin, Priority.Normal, this);

            try
            {
                accountManager = new AccountManager();
                server = new Server(accountManager);
                System.out.println("Account authentication plugin loaded.");
                System.out.println("Authentication running on :" + PORT);
            } 
            catch (IOException ex)
            {
                System.out.println("Account authentication plugin failed to load.");
                IS_RUNNING = false;
            }
	}

	@Override
	public void onDisable()
	{
            server.CloseServer();
            IS_RUNNING = false;
	}
        
        public static void AddLoginRequest(String address, String username)
        {
            RemoveTimedOutLoginRequests();
            
            LoginRequest loginRequest = new LoginRequest(address, username);
            
            synchronized(LOGIN_REQUESTS)
            {
                LOGIN_REQUESTS.add(loginRequest);
            }
        }
        
        public static boolean VerifyLoginRequest(String address, String username)
        {
            boolean returnValue = false;
            
            synchronized(LOGIN_REQUESTS)
            {
                for (LoginRequest loginRequest : LOGIN_REQUESTS)
                {
                    if (loginRequest.Verify(address, username) == true && !loginRequest.LoginTimedOut())
                    {
                        returnValue = true;
                        break;
                    }
                }
            }
            
            return returnValue;
        }
        
        public static void RemoveTimedOutLoginRequests()
        {
            ArrayList<LoginRequest> loginRequests = new ArrayList<LoginRequest>();
            
            synchronized(LOGIN_REQUESTS)
            {
                for (LoginRequest loginRequest : LOGIN_REQUESTS)
                {
                    if (loginRequest.LoginTimedOut())
                    {
                        loginRequests.add(loginRequest);
                    }
                }
            }
            
            synchronized(LOGIN_REQUESTS)
            {
                for (LoginRequest loginRequest : loginRequests)
                {
                    LOGIN_REQUESTS.remove(loginRequest);
                }
            }
        }
}
