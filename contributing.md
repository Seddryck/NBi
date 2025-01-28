# How to contribute

Third-party patches are essential for keeping **DubUrl** great. We want to keep it as easy as possible to contribute changes. There are a few guidelines that we need contributors to follow so that we can have a chance of keeping on top of things.

We love pull requests from everyone. By participating in this project, you agree to abide by the thoughtbot [code of conduct].

[code of conduct]: https://thoughtbot.com/open-source-code-of-conduct

## Getting Started

* Make sure you have a [GitHub account](https://github.com/signup/free) (It's free!)
* Read the documentation.
* Search in the issues if your problem hasn't already be submitted.
* Submit an issue (assuming one does not already exist).
  * Clearly describe the issue including steps to reproduce when it is a bug.
  * Specify the version on which this bug has been found.
* Fork the repository on GitHub

## Making Changes

* Create a topic branch from where you want to base your work.
  * This is usually the master branch.
  * Only target release branches if you are certain your fix must be on that
    branch.
  * To quickly create a topic branch based on main; `git checkout -b
    feature/my_contribution main`. Please avoid working directly on the
    `main` branch.
* Make commits of logical units.
* Make sure your commit messages are explicit.
* Make sure you have added the necessary tests for your changes.
* If possible, run _all_ the tests (for _all_ .NET versions supported) to assure nothing else was accidentally broken.
* Check the status of your build on appVeyor. 
* If something is broken, try to fix it.

## Making Trivial Changes

### Documentation

For changes of a trivial nature to comments and documentation, it is not necessary to create an issue. In this case, it is
appropriate to start the first line of a commit with '(doc)'.

## Submitting Changes

* Push your changes to a topic branch in your fork of the repository.
* Submit a pull request to the repository DubUrl (Seddryck/DubUrl).
* Update your issue to mark that you have submitted code and are ready for it to be reviewed. Include a link to the pull request in the ticket.
* At this point, you're waiting on us. We like to, at least, comment on pull requests within a few business days. We may suggest some changes or improvements or alternatives.

# Some things that will increase the chance that 

## Your issue will be quickly answered:

* Write a good description of your problem in the issue ticket
  * If you've a problem using DubUrl, post your test (not a screen capture) and explain what you're trying to achieve. Don't forget to post the error (full stack) that you're receiving!
  * If you'd like to suggest a feature, explain why you think it will be valuable and suggest a syntax.

## Your pull request is accepted:

* Link your pull request to an issue that is well documented.
* Write unit tests.
* Write the user documentation. 
