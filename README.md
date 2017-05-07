# HSP .NET API

## To use

Sign up to an account on https://datafeeds.nationalrail.co.uk/ for the username and password.
See http://wiki.openraildata.com/index.php/HSP for details of the API.

## URLs

### https://hsp-api.azurewebsites.net/api/services/{fromCRS}/{toCRS}/

Parameters:
* {fromCRS}
  * From CRS Location Code
* {toCRS}
  * To CRS Location Code

Optional Query Parameters:
* startDate
  * optional start date, if not set then current UTC time - 2hrs
* endDate
  * optional end date, if not set then current UTC time
