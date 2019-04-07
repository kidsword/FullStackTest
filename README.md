# FullStackTest
Test for Full Stack Asp.Net Core Programer

- [x] Create a site with 4 simple static pages
  - [x] Home
  - [x] Landing
  - [x] Checkout Checkout
  - [x] Order confirmation
  
- [x] Create a Web API to collect client behavior

  - [x] Behavior must have the following data
    - [x] Ip
    - [x] Page Name
    - [x] Browser
    - [x] Page Parameters
  - [x] Must be Stored in a RabbitMQ Queue
- [x] Create a JavaScript that collects and submits the Data to the API
  - [x] It should be a simple JavaScript that collects the data from the Browser and send it to Behavior API
  - [x] Must be delivered to all Static Pages

- [x] Create a Robot that consumes the queue in RabbitMQ
  - [x] You should read the Events in the RabbitMQ Queue
  - [ ] Save the Data in two different Infras
    - [ ] SQL Server
    - [x] File Server (CSV or JSON)
