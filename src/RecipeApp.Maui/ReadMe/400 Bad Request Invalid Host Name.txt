
Make Sure CoreApi (any api) in IISExpress is also running under 127.0.0.1 as well as localhost

ERROR
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd">
<HTML><HEAD><TITLE>Bad Request</TITLE>
<META HTTP-EQUIV="Content-Type" Content="text/html; charset=us-ascii"></HEAD>
<BODY><h2>Bad Request - Invalid Hostname</h2>
<hr><p>HTTP Error 400. The request hostname is invalid.</p>
</BODY></HTML>

FIX:  Modify applicationhost.confiv in .vs folder of solution folders.. Bindings section


https://mattruma.com/debugging-net-core-api-on-android-device/
