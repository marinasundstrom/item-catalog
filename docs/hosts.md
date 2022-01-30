# Hosts

In Nginx, these server names have been configured: 

* ```localhost``` - Proxying to AppService, Azurite
* ```identity.local``` - Proxying to IdentityService

Below there is a walkthrough for configuring ```identity.local```.

## Add ```identity.local``` to ```host``` file

In order for the host to recognize the ```ìdentity.local``` domain, to map it to the right IP address, you have to add it to the ```host``` file:

### On macOS:

Edit the ```/etc/hosts``` file.

Add this line to the file:
```
127.0.0.1   identity.local
```

Then restart the DNS:

```sh
sudo killall -HUP mDNSResponder 
```

### Other platforms

Read this [guide](https://www.howtogeek.com/howto/27350/beginner-geek-how-to-edit-your-hosts-file/) for configuring on Windows and Linux.

### Conclusion

You should now be able to reach the identity server site in your browser by navigating to [```https://identity.local:8080```](https://identity.local:8080).

Please be aware that you have to configure the certs.