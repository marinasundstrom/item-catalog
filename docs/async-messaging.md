# Asynchronous messaging

The Web API communicates with the Worker by sending messages using asynchronous messaging, facilitated by the MassTransit framework, with RabbitMQ as transport.

In the WebApi and Worker projects respectively, the Consumers folders house the consumers for the asynchronous messaging between the services. Think of these as the message or event handlers. 

All the Message types are in the Contracts project.

Remember hat messages can be received by multiple consumers. There is also an option to do RPC calls. This project is not currently using that,

In the Worker project, there is a Notifier service that wraps the MassTransit stuff that publishes an event when the recurring task executes (every minute).  