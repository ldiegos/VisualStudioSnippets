When you need to have a project with dll or standard libraries, the best way to have it all at GIT is using the subtrees.
There is other option, submodules, but is harder to understand.

Take in mind that using subtrees, you will own the copy of the dll/module, so all changes that you make to the dll, 
will be store in the main project git. After the store into the master branch of the main git. You must send the modification
to the master branch of the dll, if you want to share your improvement with others.

==============================================================
1-Just create in your main project a folder which will contain the libraries.
-FileMaintenance = Main project.
-ExternalLibs = folder to contain the libraries or modules.
Example:
FileMaintenance\ExternalLibs

2-Open git bash console, better than the command prompt from windows. (git bash must be installed.)
GeMe@GeMe-Win2012DataCenter MINGW64 ~
$

3-Go to the main project folder at the bash: Note: If you are on windows, change the slash from the back to the front.
Example: <path>\FileMaintenance -><path>/FileMaintenance
cd <path>/FileMaintenance

If your main project folder is under git control vertion, the bash console will show the "(master)"
GeMe@GeMe-Win2012DataCenter MINGW64 /d/Desarrollo/TFS-Git/Personal/FileMaintenance (master)
$

4-Start adding all the dll that you want to be linked to the project and also it must be under git.
git remote add -f <alias> <url to git dll>
Example: git remote add -f GMS_LIB_DATA_ACCESS http://192.168.1.6:9090/tfs/Personales/_git/GMS_LIB_DATA_ACCESS

The alias is goint to be very usefull at the next commands.

5-Start downloading the master branch from the dlls.
git subtree add --prefix <folder>/<alias> <local branch> <remote branch>
Example: git subtree add --prefix ExternalLibs/GMS_LIB_DATA_ACCESS GMS_LIB_DATA_ACCESS master

6-When the extraction finish, you will have copy of the dll in the given folder.
Example: D:\Desarrollo\TFS-Git\Personal\FileMaintenance\ExternalLibs\GMS_LIB_DATA_ACCESS

7-Lets start using, in my case Visual Studio 2015.

8-Add a project folder into the main project which will store the dll.
Example:
-Solution 'FileMainentence'(12 projects)
--ExternalLibs	

9-Add a existing project into the new folder.
Example:
-Solution 'FileMainentence'(12 projects)
--ExternalLibs	
---GMS_LIB_DATA_ACCESS

# FROM "HERE"(10) TO "THERE"(13) can be use the Visual Studio to stage and commit all the changes to the dll (locally).
# ------------------------"HERE"-------------------------------------
10-When you make changes to your copy of the dll, the git will treat as if it were make on the main project.
You could use the "git status" command to show the files that had modifications.
-----------------------------------------------------------------------------------------------
$ git status
On branch master
Your branch is up to date with 'origin/master'.

Changes not staged for commit:
  (use "git add <file>..." to update what will be committed)
  (use "git checkout -- <file>..." to discard changes in working directory)

<b>        modified:   ExternalLibs/GMS_LIB_DATA_ACCESS/GmsLibDataAccessSqlServer.cs<b> <--RED

no changes added to commit (use "git add" and/or "git commit -a")
-----------------------------------------------------------------------------------------------

11-let add the modification to the staged with "git add -A". This will staged all the files.
GeMe@GeMe-Win2012DataCenter MINGW64 /d/desarrollo/tfs-git/personal/filemaintenance (master)
$ git add -A

12-Use git status again.
-----------------------------------------------------------------------------------------------
$ git status
On branch master
Your branch is up to date with 'origin/master'.

Changes to be committed:
  (use "git reset HEAD <file>..." to unstage)

        modified:   ExternalLibs/GMS_LIB_DATA_ACCESS/GmsLibDataAccessSqlServer.cs <--GREEN
-----------------------------------------------------------------------------------------------

13-Then lets make commit the change into git
GeMe@GeMe-Win2012DataCenter MINGW64 /d/desarrollo/tfs-git/personal/filemaintenance (master)
$ git commit -m"<Comment>"
----------------------------------------------
[master 0364562] <commen>
 1 file changed, 1 insertion(+), 1 deletion(-)
----------------------------------------------

# ------------------------"THERE"-------------------------------------
NOTE: This step can't be done from Visual Studio 2015
14-At this moment, you will decide if you want to publish/share the modification or just wait until your update policy/agreement expires.

15-To publis/share the changes, you must use "git subtree push".
GeMe@GeMe-Win2012DataCenter MINGW64 /d/desarrollo/tfs-git/personal/filemaintenance (master)
$ git subtree push --prefix=<folder>/<alias> <branch> master

Example: git subtree push --prefix=ExternalLibs/GMS_LIB_DATA_ACCESS GMS_LIB_DATA_ACCESS master

16-There is no problem to repeat the command, Git is clever and notify us, that every this is just commited:
GeMe@GeMe-Win2012DataCenter MINGW64 /d/Desarrollo/TFS-Git/Personal/FileMaintenance (master)
$ git subtree push --prefix=ExternalLibs/GMS_LIB_DATA_ACCESS GMS_LIB_DATA_ACCESS master
git push using:  GMS_LIB_DATA_ACCESS master
Everything up-to-date
