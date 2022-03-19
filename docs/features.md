# Features

This document explains the features of this solution.

The logic is implemented in multiple services.

## Items

Add Items with pictures to the Catalog.

User can comment on the Items, whether their own or others. Creators get notified when someone else comments on their Items.

This is part of AppService.

## Notifications

The ability to publish notifications is built into the app and used by Items. It is its own service.

It even sends an email to the receiving User.

## Users

In order to access functionality in this app, the User has to be logged in.

Administrators can create and manage Users. The service in charge is IdentityService.

## Worker (Ping)

The Worker runs a recurring task every minute that emits a message that is handled similar to the feature above. 

The message is published so that anybody who wants can receive it.
