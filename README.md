# CallawayPreOwnedService
## Purpose: 
### Windows Service that polls the Callaway PreOwned Service Product Search endpoint for wanted products and sends out e-mail alert when one of the target products is found.

## Stack:
### .NET Core 3.1
### SendGrid (E-Mail)

## TODO:
- Version 1.0:
   - Add Installation and Setup instructions to README
   - Add to Config
      - API address and endpoints
      - Polling interval
- Version 2.0:
   - Include Handedness in Product definition (?)
   - File DB implementation to track emails sent per product
   - Containerize and look for cloud deployment options
   - Refactor to support centralized deployment with support for mutiple users
- Backlog:
   - Web UI to configure settings and add alerts/users


