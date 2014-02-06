## ASP.NET Guidelines

### MVC

 * use web.config for configuration differences as opposed to
   `if(TEST_MODE)` checks; the VS publish system will do web.config
   modifications at publish time
 * prefer `HttpContext.IsDebuggingEnabled` over `TEST_MODE`
 * don't expose database objects to templates. Refactoring tools like
   "Rename" don't work into templates, and it's easily to accidentally
   fetch the entire database. For each template, make a little class
   (a "view model" or Data Transfer Object (DTO)) that has all the
   data you want to display
 * don't use interfaces as DTOs. The razor system doesn't like it, and
   will bail with "The property X could not be found." The reasoning: 
   http://stackoverflow.com/a/5491525/311289
 * use [FluentValidation][] to make validators for your DTOs
 * put templates named after the DTO type in
   `~/Views/Shared/DisplayTemplates` and
   `~/Views/Shared/EditorTemplates` to control the output when calling
   `Html.DisplayFor` or `Html.EditorFor`
 * `EditorFor` can only be called _once_ per object! It keeps a list
   internally to make sure you don't render twice. This prevents any
   kind of template inheritance; you can still do it, but you have to
   render the "base" template using `RenderPartial`
 * use `[DataType]`, `[Display]`, and `[UIHint]` annotations with
   `HtmlHelper.DisplayFor` to further control output
 * use [NInject][] to handle object instantiation; enables making many
   small classes/interfaces and then composing them at runtime without
   writing a thousand lines of argument passing
 * if you want to loop over an array more than once (i.e. once to draw
   a row for each item, once for a sum at the bottom), use an
   `ICollection` in the view model. This will ensure we don't do
   duplicate queries.
 * use [MiniProfiler][] to get alerted about duplicate/slow queries. Try
   to run a fixed number of queries per HTTP request.
 * try to avoid `HtmlHelper.RenderAction`, it basically runs the
   entire request pipeline again
 * rarely do you want a view that accepts an `ICollection<T>`,
   frequently you end up wanting metadata with that collection and
   it's nice to have a place to put it from the start



[FluentValidation]: http://fluentvalidation.codeplex.com/
[NInject]: http://www.ninject.org/
[MiniProfiler]: https://www.nuget.org/packages/MiniProfiler


### WebForms

 * prefer pages over controls; it's actually pretty rare that we need
   to re-use UI bits, and usually those can be easily re-used in
   master page templates
 * make a dummy `IMyControlOrPage`, then use extension methods on that
   to get one piece of code that applies to `Control`s and `Page`s:
    * `class MyControl : IMyControlOrPage` - make all user controls
      extend `MyControl`
	* `class MyPage : IMyControlOrPage` - make all pages extend
      `MyPage`

### IIS Express

It's nice to bind your dev IIS server to your computer's private IP, not just localhost

http://stackoverflow.com/questions/14881515/vs2012-iis-express-browse-web-site-with-ip-address-rather-than-localhost

* edit `Documents/IISExpress/applicationhost.config` - look to the
  `<bindings>` section for your site, this controls both IP binding
  and host header config.
* add permissions; start cmd.exe as administrator

        netsh http add urlacl url=http://${HOST}:${PORT}/ user=everyone
        netsh advfirewall firewall add rule name="IISExpressWeb" dir=in protocol=tcp localport=${PORT} profile=private remoteip=localsubnet action=allow

* edit `web.config` to show remote errors

        <system.web>
          <customErrors mode="Off"/>
