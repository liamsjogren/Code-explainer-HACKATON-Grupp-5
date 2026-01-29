# Code explainer

### To add API key for development, follow these steps

1. Go to https://platform.openai.com/ and Log In
2. On the sidebar, go to API Key
3. Press create new secret key
4. Name it something and select default project
5. Copy+paste the API Key into "Properties/appsettings.local.json" between the ""
6. (IMPORTANT) When you commit, make sure that "Properties/appsettings.local.json" is NOT staged
