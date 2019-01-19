package account;

import java.io.*;
import java.net.*;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Server extends Thread
{
    private AccountManager accountManager;
    private ServerSocket serverSocket;
    
    public Server(AccountManager accountManager) throws IOException
    {
        this.accountManager = accountManager;
        
        // Open listening socket
        this.serverSocket = new ServerSocket(Main.PORT);
        
        // Begin listening
        this.start();
    }
    
    public void Login(Socket clientSocket, String username, String password)
    {
        if (accountManager.AccountVerifyLogin(username, password))
        {
            System.out.println("New user login: " + username);
            Send(clientSocket, "LOGIN OK");
            
            String address =(((InetSocketAddress) clientSocket.getRemoteSocketAddress()).getAddress()).toString().replace("/","");

            
            Main.AddLoginRequest(address, username);
        }
        else
        {
            Send(clientSocket, "LOGIN ERROR");
        }
        
        CloseSocket(clientSocket);
    }
    
    public void Register(Socket clientSocket, String username, String password)
    {
        if (!accountManager.AccountExists(username))
        {
            if (username.length() > 3 && password.length() > 3)
            {
                System.out.println("New user registered: " + username);
                            
                accountManager.AddNewAccount(username, password);
                Send(clientSocket, "REGISTER OK");
            }
            else
            {
                if (username.length() < 3)
                {
                    Send(clientSocket, "REGISTER ERROR Username is too short.");
                }
                else
                if (password.length() < 3)
                {
                    Send(clientSocket, "REGISTER ERROR Password is too short.");
                }
            }
        }
        else
        {
            Send(clientSocket, "REGISTER ERROR Username already exists.");
        }
        
        CloseSocket(clientSocket);
    }
    
    private void CloseSocket(Socket clientSocket)
    {
        try
        {
            clientSocket.close();
        }
        catch (IOException ex)
        {
            Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public void CloseServer()
    {
        try
        {
            serverSocket.close();
        }
        catch (IOException ex)
        {
            Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public void Send(Socket clientSocket, String message)
    {
        try
        {
            PrintWriter pw = new PrintWriter(clientSocket.getOutputStream(), true);
            pw.println(message);
        }
        catch (IOException ex)
        {
            Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public void run()
    {
        while (Main.IS_RUNNING)
        {
            try 
            {
                Socket clientSocket = serverSocket.accept();
                
                // login or register within x millis
                clientSocket.setSoTimeout(150);
                
                try 
                {
                    BufferedReader input = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
                    
                    String line;
                    
                    try
                    {
                        // Read from socket until it closes.
                        while ((line = input.readLine()) != null)
                        {
                            int spaceCount = line.length() - line.replace(" ", "").length();

                            // line is the incoming command
                            if (line.startsWith("LOGIN "))
                            {
                                String login = line.substring(6);
                                
                                // name password + the entire command should only have two spaces
                                if (login.trim().split(" ").length >= 2 && spaceCount == 2)
                                {
                                    String username = login.split(" ")[0].trim();
                                    String password = login.split(" ")[1].trim();
                                    
                                    if (username.length() > 0 && password.length() > 0)
                                    {
                                        Login(clientSocket, username, password);
                                    }
                                    else
                                    {
                                        clientSocket.close();
                                    }
                                }
                                else
                                {
                                    clientSocket.close();
                                }
                            }
                            else
                            {
                                if (line.startsWith("REGISTER "))
                                {                                    
                                    String register = line.substring(9);
                                    
                                    // name password + the entire command should only have two spaces
                                    if (register.trim().split(" ").length >= 2 && spaceCount == 2)
                                    {
                                        String username = register.split(" ")[0].trim();
                                        String password = register.split(" ")[1].trim();
                
                                        if (username.length() > 0 && password.length() > 0)
                                        {
                                            Register(clientSocket, username, password);
                                        }
                                        else
                                        {
                                             clientSocket.close();
                                        }
                                    }
                                    else
                                    {
                                        clientSocket.close();
                                    }
                                }
                                else
                                {
                                    clientSocket.close();
                                }
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        // Socket closed
                        // Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
                    }
                }
                catch (IOException ex)
                {
                    Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
                }
            } 
            catch (IOException ex)
            {
                Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }
}