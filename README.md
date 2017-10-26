# sfmd: Salesforce metadata utility
A .Net client to query and modify Salesforce metadata from the command-line. Initially limited to Custom Object metadata only

## Configuration
Edit the `App.config` file and configure your username, password and token. You're ready to use sfmd

## Usage

### Reading metadata

To get custom object metadata (including standard objects too), type

`sfmd get -o Account Contact Lead`

### Creating metadata

To create a custom object, type

`sfmd create -o <object name> -l <object label> -p <plural object label>`

for instance:

`sfmd create -o Test -l Test -p Tests`

This will create a new Custom Object named "Test__c", with a label of "Test" and a plural label of "Tests". It will contain a single custom field, called "Test Name", to name your object


### Deleting metadata
To delete a custom object, type

`sfmd delete -o <object types>`

for instance:

`sfmd delete -o Test__c`

Use carefully! There are no confirm options!

For more info, visit: https://blog.mkorman.uk/integrating-with-metadata-api/
