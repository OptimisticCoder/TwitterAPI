# TwitterAPI

A class to make integrating with the new Twitter API easy.

Twitter API 1.1 requires you to create API keys on the developer site.

1. Login to Twitter Dev site with the account you want to use. [https://dev.twitter.com/](https://dev.twitter.com/)
2. Go to "My Applications" [https://dev.twitter.com/apps](https://dev.twitter.com/apps)
3. Create a new Twitter app. Fill in all fields, but leave Callback_Url blank.
4. After the app is created, go to its detail page and create an access token for it.
5. You need **Consumer Key**, **Consumer Secret**, **Access Token**, and **Access Token Secret**. (In that order)

NOTE: Sometimes Twitter is slow or down, so it is best to consume the Tweet class from a web service which is called from JavaScript after the page has loaded to reduce page load time.
