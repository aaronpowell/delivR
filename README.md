# DelivR

DelivR is a port of [Delivery.js](https://github.com/liamks/Delivery.js), a Node.js library for sending files over persistent connections, such as websockets or AJAX long-polling.

DelivR aims to bring the same concepts to the .NET platform, built on top of [SignalR](https://github.com/SignalR/SignalR/), a .NET async signaling library

# Get it on NuGet

*Soooooon :P*

# Usage

## Server

To create your own file sending you need to inhert from the `DelivR.FileConnection` class:

    public class Basic : FileConnection
    {
        
    }
    
This is a layer on top of the [PersistentConnection](https://github.com/SignalR/SignalR/wiki/PersistentConnection) that SignalR provides with some helpful methods for sending images. To send an image to a particular client you can do this:

    protected override Task OnConnectedAsync(IRequest request, IEnumerable<string> groups, string connectionId)
    {
        this.SendImage(connectionId, @"c:\Path\To\Image.png");
        return base.OnConnectedAsync(request, groups, connectionId);
    }

You also need to register your route still as per the PersistentConnection documentation.

## Client

On the client add a reference to the `delivR.js` file included in the package and then create a new instance of `DelivR`:

	<script>
		$(document).ready(function () {
			var basic = new DelivR('basic');
		});
	</script>
	
You need to pass in the name for the route, what you gave it in the route definition so that DelivR knows where to find it ;).

Then you need to listen to the events you want like so:

	basic.on('receive', function(e, file) {
		//do cool stuff
	});
	
The first argument is the type of message to listen for, with `receive` being the message type emitted when you use the `SendImage` methods on the server.

Finally to display the image you can interact with the file provided like so:

	basic.on('receive', function (e, file) {
		$('<img />')
			.attr('src', file.dataUri())
			.appendTo(document.body);
	});
	
Done!

# License

[MIT](https://github.com/aaronpowell/delivR/blob/master/LICENSE.md)