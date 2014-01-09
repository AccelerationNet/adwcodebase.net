# Versioning

It's very helpful to keep a summary of what's changing in our
software. This can be done with trac versions and milestones, but
that's proven too expensive to deal with. A lightweight solution
inspired by open source projects using simple text files.

Having these as files in git vs trac wiki pages gives a nice place to
change ticket statuses, and is more convenient for developers to edit.

These files are very helpful for Rebecca to summarize what's new for
clients.

## Procedure

 1. during development, keep a ReleaseNotes.md file, which has a
    high-level changelog and ticket references:

        # Version 0.3.1
    
         * add feature X #312
	     * bugfix Y #315


 2. add entries to the releasenotes and use the commit message to mark
    those tickets as `needs_publish` (usually, although sometimes we
    publish progress on an open ticket)

        update release notes

        ready #315
		ready #312

 3. when you publish, make a new `git tag` with the version at the top
    of ReleaseNotes.md
 4. after publish, edit release notes to add the publish date:
 
        # Version 0.3.1 - 2014-01-07
    
         * add feature X #312
	     * bugfix Y #315
		 
 5. use that commit message to close any tickets that need it, using
    the releasenotes as a handy index to what's ready to go:

        published

        fix #315
		fix #312

 6. when we have something ready to go, make the next version in release notes

        # Version 0.3.2

         * add feature Z #316
		 
        # Version 0.3.1 - 2014-01-07
    
         * add feature X #312
	     * bugfix Y #315

## Problems

### Wanting multiple versions in the same repo

Cases:

 * Pharmisync importer, api, frontends in the same repo, all with
   logically different release notes and versions
 * Commitments ext vs rsh in the same repo, also logically different
   release notes and versions

Possible solutions:

 * add a prefix to the version number, keep separate (eg: `rsh-0.3.1`
   vs `ext-0.3.1`)
   * pretty easy
   * gets a little confusing having multiple release notes files, need
     one for each user-facing system
 * more git repos
   * submodule madness
