After some time, I try to get back into MVVM / Avalonia. :)

This is just a small project I setup for myself as a little challenge. It should do the following:

It acts like a file browser, but for images. 
After selecting a root directory for images, it checks all the files and filters out the images. 
Those images then get stored in data models, which contain some of the EXIF and file information.
The program then generates thumbnails / previews for those images, which will be shown in the List of images.

What it will do:
It allows for custom attributes of those images, like a description or a custom name.
It will also support a virtual folder directory, to move images into and so on.
None of the edits will be done to the actual images, instead they will be saved (probably in an SQLIte database) and just displayed that way.
It will also allow for the conversion of formats, which are normally not supported by Avalonia (like HEIC), in which case it will create a full scale converted copy + the thumbnail.

Currently, the thumbnails are saved in a folder that has excluded_ in its name, which will then be excluded from being reprocessed (since the base directory is currently also the directory, the thumbnail image is placed in).
Those things, just as the maximum preview image size, will be configurable once I get to it.

The conversion option should also be callable outside of the image adding process, because why not?


Yes, it is not a particularly useful bit of software, but it is a nice challenge and something I would just like to do.
Feel free to use the source in any open source project of your choosing, although you might want to rethink that for most parts since its not particularly optimized :)
