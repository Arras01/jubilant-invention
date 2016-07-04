Compilation instructions (ones that work for me, at least):
--Open in Visual Studio 2015
--Set release target to Release and platform to x64
--if x64 is not an option in the dropdown, click Configuration Manager, click the dropdown for Platform,
	select New, select x64 and hit OK
--compile and it should work
--sometimes it randomly gives a "missing file" exception from one of the libraries when you compile, 
	just stop debugging and compile again and it should fix itself
--arrow keys to look around

Implemented functionality:
basic path tracer with russian roulette, next event estimation and importance sampling

note: a lot of very bright colored noise appears, I'm not sure what causes it. I wasn't able to figure it out, unfortunately.

Sources:
--On fast Construction of SAH-based Bounding Volume Hierarchies, Wald, 2007
--scratchapixel
--presentation slides
--various webpages such as StackExchange