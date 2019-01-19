package account;

import java.time.Duration;
import java.time.LocalDateTime;

public class LoginRequest
{
    private String address;
    private String username;
    private LocalDateTime loginTimestamp;
    
    public LoginRequest(String address, String username)
    {
        this.address = address;
        this.username = username;
        this.loginTimestamp = LocalDateTime.now();
    }
    
    public boolean Verify(String address, String username)
    {
        boolean returnValue = false;
        
        if (this.address.equals(address) && this.username.equals(username))
        {
            returnValue = true;
        }
        
        return returnValue;
    }
    
    public boolean LoginTimedOut()
    {
        boolean returnValue = false;
        
         LocalDateTime timestampNow  = LocalDateTime.now();
         
         long timeDifference = Duration.between(loginTimestamp, timestampNow).toMillis() / 1000;
         
         if (timeDifference > Main.LOGIN_TIMEOUT)
         {
             returnValue = true;
         }
         
         return returnValue;
    }
}
