package account;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

public class AccountManager
{
    private JSONArray jArrAccounts;
    
    public AccountManager()
    {
        JSONParser jsonParser = new JSONParser();
        
        try
        {
            jArrAccounts = (JSONArray)jsonParser.parse(ReadAccounts());
        }
        catch (ParseException ex)
        {
            // Using empty JSONArray
            System.out.println("Using empty accounts file.");
            jArrAccounts = new JSONArray();
        }
    }
    
    public void AddNewAccount(String username, String password)
    {
        JSONObject account = new JSONObject();
        account.put("password", password);
        account.put("username", username.toLowerCase());
        
        jArrAccounts.add(account);
        
        WriteAccounts();
    }
    
    public boolean AccountVerifyLogin(String username, String password)
    {
        boolean returnValue = false;
        
        for (Object account:jArrAccounts)
        {
            if (((JSONObject)account).get("username").equals(username.toLowerCase()))
            {
                if (((JSONObject)account).get("password").equals(password))
                {
                    // Username and password match.
                    returnValue = true;
                }
                
                break;
            }
        }
        
        return returnValue;
    }
    
    public boolean AccountExists(String username)
    {
        boolean returnValue = false;
        
        for (Object jArrAccount : jArrAccounts)
        {
            JSONObject account = (JSONObject)jArrAccount;
            if (account.get("username").equals(username.toLowerCase()))
            {
                returnValue = true;
                
                break;
            }
        }
        
        return returnValue;
    }
    
    private String ReadAccounts()
    {
        // Attempt to read accountFile. If it doesn't exist then create one and return empty.
        String returnValue = "";
        
        File file = new File(Main.ACCOUNTS_FILE);
        if(file.exists() && !file.isDirectory())
        { 
            try
            {
                returnValue = new String(Files.readAllBytes(Paths.get(Main.ACCOUNTS_FILE)), StandardCharsets.UTF_8);
            }
            catch (IOException ex)
            {
                System.out.println("Unable to read accounts file.");
            }
        }
        else
        {
            try
            {
                file.createNewFile();
            }
            catch (IOException ex)
            {
                System.out.println("Unable to create accounts file.");
            }
        }
        
        return returnValue;
    }
    
    private void WriteAccounts()
    {
        BufferedWriter writer = null;
        
        String accounts = jArrAccounts.toJSONString();

        try
        {
            FileWriter fileWriter = new FileWriter(Main.ACCOUNTS_FILE, false); // DON'T APPEND
            writer = new BufferedWriter(fileWriter);
            writer.write(accounts);
            writer.close();
        }
        catch (IOException ex)
        {
            System.out.println("Unable to write to accounts file.");
        }
    }
}
