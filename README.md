# EldenRing Auto BackUp Windows Service

## EldenRing SaveFile Watcher
In the event of a hack attack destroying a saved file, you can use the latest version of backup to revert.  


When a save is performed, the service detects it and backs up the saved file(appdata/roaming/EldenRing).  
The default number of saves is 100.  
Old ones are deleted sequentially.  


# How to install
### 1. config settings
  open the EldenRingAutoBackUpService.exe.config  
  You can modify the config file to change the number of saved files or set the backup save path.  
  
  ~~~ 
  <add key="BackUpPath" value="C:\EldenRingAutoBackUpFiles"/>
  <add key="MaxBackUpFileCount" value="100"/>
  ~~~


### 2. Register Windows Service  

  Run cmd as administrator.  
  >   
     sc create "EldenRing Auto BackUp Service" binpath="C:\EldenRingAutoBackupService\EldenRingAutoBackUpService.exe" start=auto
  
   ※ In binpath, enter the full path to the program file you downloaded.

### 3. Start the Service  

  > 
     sc start "EldenRing Auto BackUp Service"  
     
     
   <img width="992" alt="image" src="https://user-images.githubusercontent.com/39667272/159204014-47c8c5e7-0927-4d7a-862d-844dd5901237.png">

# Backup file results  

<img width="846" alt="image" src="https://user-images.githubusercontent.com/39667272/159203546-c1dd2795-efef-4a76-908a-91a990493506.png">

# __Warning!__  
__Shut down EldenRing as soon as you receive a save destruction hack attack.__  
__The file is overwritten with an older version of the latest backup file.__  

__※ Because it is saved immediately when you die__  
__If you die more than the set number of saves, the file cannot be recovered.__  




# How to UnInstall
### 1. Delete the registered service through cmd.

> 
    sc delete "EldenRing Auto BackUp Service"

### 2. Delete the service file.


