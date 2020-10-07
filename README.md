# CallawayPreOwnedService
## Purpose: 
### Windows Service that polls the Callaway PreOwned Service Product Search endpoint for wanted products and sends out e-mail alert when one of the target products is found.

## Stack:
### .NET Core 3.1
### SendGrid (E-Mail)


## TODO:
- Add configurable parent-product search (replace MensRightApexIronParams class with something configurable: Product definition stored in Json with a key that can be loaded dynamically based on a setting)
- Add configurable target product search
-  Make API address and endpoints config settings