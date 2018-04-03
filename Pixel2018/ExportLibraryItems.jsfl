/*
* JSFL ExportImage
* Author: Hasan Otuome
* hasan.otuome.com
* Version: 1.0
* June 19, 2006
*/
fl.outputPanel.clear();
 
//declare some variables
var doc = fl.getDocumentDOM();
var lib = doc.library;
 
//library variables
var imgs = lib.items;
var img;
var filename = new Array();
 
//counter variables
var i;
var l=0;
 
//folder variables
var browser;
var temp;
var folder;
 
//for dealing with document selection
var selectedImage = new Array();
 
//get info about images in library
for (i=0; i < imgs .length; i++)
{
	img = imgs[i];
	if (img.itemType == "movie clip")
	{
		filename.push( img.name );
	}
}
 
//select the destination folder
browser = decodeURI( fl.browseForFolderURL("Select a folder.") );
folder  = browser + "/";
 
//only try to export the images if a destination folder's been chosen
if (browser)
{
	for (i=0; i < filename.length; i++)
	{
		lib.selectItem( filename[i], true );
		var selItems = lib.getSelectedItems();
		lib.addItemToDocument( {x:0, y:0} );
		l=1;
		fl.trace( folder + filename[i] );
		exportImage( filename[i] );
	}
}
 
//exports each image as a PNG
function exportImage( file )
{
	if (doc.selection[0] || doc.selection.length > 1)
	{
		var selectedImage = doc.selection;
		var type = selectedImage[ 0 ].elementType;
		if (type == "instance")
		{
			var sel = selectedImage[ 0 ];
			var w = sel.width;
			var h = sel.height;
			if (file.indexOf(".") != -1)
			{
				var temp = file.split( "." );
				var file = temp[ 0 ];
			}
			if (l == 1)
			{
				doc.clipCut();
			} 
			else 
			{
				doc.clipCopy();
			}
			fl.createDocument();
			doc = fl.getDocumentDOM();
			doc.clipPaste();
			doc.selectAll();
			var sel = doc.selection[ 0 ];
			doc.width = Math.floor( w );
			doc.height = Math.floor( h );
			doc.moveSelectionBy( {x:-sel.left, y:-sel.top} );
			doc.selectNone();
			doc.exportPNG( folder + filename[i], true, true );
			fl.getDocumentDOM().close( false );
		}
	}
}