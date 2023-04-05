# Xero Presence
Discord Rich Presence for Xero  
Every 30 seconds it updates your current game state

## Information
- The session id will expire once your current session has ended (After xero detects that you are offline)
- This means you may have to get a new session id each day
- I'll try to automate it some day

## Tutorial
1. Open Firefox or Chrome (tutorial is written using chrome)
2. Open xero.gg and login with the account that has your main account as friend and only your main account
3. Press F12 to bring up the console
4. Head over to "Application"
5. Click on Cookies and select https://xero.gg/
6. Copy the value from xero_session_ssl
7. Paste it into the program
8. Enter the nickname of the account that has your main account in the friendlist
9. Click on "Start Presence"
10. Should work now