# CallawayPreOwnedService
## Purpose: 
### Windows Service that polls the Callaway PreOwned Service Product Search endpoint for wanted products and sends out e-mail alerts when one of the target products is found.

## Stack:
### .NET Core 3.1
### SendGrid (E-Mail Functionality)
___
## Version 1.0 - Initial Release - General Info, Installation and Configuration
### General Information
- The service is set to poll the Callaway Pre-Owned site every 60 minutes in search of the products configured in the configuration file (appsettings.json). 
- If there are any matches to the wanted products list (also configured in the apsettings.json), it will send an e-mail from/to the e-mail addresses configured in the service with subject line Callaway Pre-Owned Service Alert: Wanted Product(s) Available with information on the product available and a direct link to said product.
- The service uses SendGrid  (www.sendgrid.com) which is a service that will handle the e-mail sending functionality. In order to get the e-mails, you'll need to register a (free) account and reqest an API Key (also free). See Configuration section for details.
- Since this is a Windows Service, it will only run while your PC is awake. When your PC goes to sleep or is shut-off, the service will not work.
- The Service is state-less, meaning it has no awareness of what it has done previously. If you have a Product search that hits one of your matches, you will continue to get e-mails about it until the product is no longer in inventory, or you refine the product matching criteria.
- If you make any type of configuration update to the appsettings.json file, you'll need to restart the service for the settings to kick in. 
### Installation
- Make sure you have DotNet Core 3.1 Runtime (Desktop) installed: https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.9-windows-x64-installer
- Download release package from GitHub Releases (CallawayPreownedService.zip): https://github.com/LRizoJr/CallawayPreOwnedService/releases/tag/1.0
- Extract zip file contents to a folder
- Navigate to extracted folder and run the InstallService.cmd file as an Administrator (this will install the "CallawayPreOwnedService" service on your PC)

### Configuration
- Navigate to the CallwayPreOwnedService folder and open the appsettings.json file  to fill in the required sections you must configure:
   - Email Options: EmailFrom, EmailTo, EmailToName
   - ProductSearchOptions: This is a list of json objects that tells the service what clubs to search for in the Callaway PreOwned Site. It requires three fields: Product ID, "CGIDs", and GenderHand value ("Mens%2FRight" for Right Handed Men's or "Womens%2FLeft" for Women's Left handed, for example). The service comes with two club search out of the box: 2014 Apex Irons (Men's Right) and Taylormade M4 Woods.
      - In order to figure out the value of these fields, manually navigate to the Product Page for the product you're interested in on the Callaway PreOwned Site and turn on the Console (F12 in Google Chrome). Then, navigate to the Network tab and refresh the page. Using the search field near the top left of the console (see 1 in screenshot below), search for "product-variant", this will apply a filter to all the network requests. Then click on the Product-VariantData request (see 2 in screenshot below), and this will display the request information, including the Request URL, where you will have the pid (Product ID), cgid (CGID) and genderHand (GenerHand) field values required for the product search.
      ![Product Keys Lookup](https://i.imgur.com/whji29U.png)
   - WantedProductOptions: Finally, this is a list of json objects which are the products you're interested in. Out of the box, the config settings are looking for a 4-iron from the 2014 Apex Irons set and a 3 Wood from the Taylormade M4 Faireway Wood set. 
      - Note that out of the box the WantedProductOptions only use ParentProductID and Club when looking for matching products, but you can add any of the following fields as well, just be aware that the string match must be exact (not case-sensitive):
        - ShaftFlex 
        - ShaftType
        - LieAngle
        - Length
        - Condition
- In order for the e-mail alert functionality to work, you must create a (free) SendGrid account (https://sendgrid.com/) and request an API key (after you register and log in, go to Settings > API Key > Create API Key). SendGrid is the service used for e-mail generation from the CallawayPreOwnedService. You must then add this API key as an environment variable to your PC called SENDGRID_API_KEY (instructions here: https://superuser.com/questions/949560/how-do-i-set-system-environment-variables-in-windows-10).
- Once you've done all of the above, you should be able to Start the service from your Windows Services list. You will see log entries in the Windows Event Viewer (Windows Logs > Application) with informational messages and any errors that may occur under the Event Source: CallawayPreOwnedService. 
___
## TODO:
- Version 1.1:   
   - Add to Config:
      - API address and endpoints
      - Polling interval
- Version 2.0:
   - Include Handedness in Product definition (?)
   - File DB implementation to track emails sent per product
   - Containerize and look for cloud deployment options
   - Refactor to support centralized deployment with support for mutiple users
- Backlog:
   - Web UI to configure settings and add alerts/users


