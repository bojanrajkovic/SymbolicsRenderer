var SymbolicExpressionRenderer = (function() {
    function SymbolicExpressionRenderer() {}

    // We can use this CSS class to style our renderer's container if we want to.
    SymbolicExpressionRenderer.prototype.cssClass = "renderer-third-party-symbolic-expression";

    // This is the name of our representatoin--this will be shown by Workbooks next to the
    // result object.
    SymbolicExpressionRenderer.prototype.getRepresentations = function() {
        return [
            { shortDisplayName: "Symbolic Expression" }
        ];
    };

    // Binds some render state to our renderer--this will be the object to be rendered
    SymbolicExpressionRenderer.prototype.bind = function(renderState) {
        console.log("SymbolicExpressionRenderer: bind: %0", JSON.stringify(renderState));
        this.renderState = renderState;
    };

    // Called when it's time to render the object. The passed target is our target DOM element,
    // and we received the object to render earlier via the bind method.
    SymbolicExpressionRenderer.prototype.render = function(target) {
        console.log("SymbolicExpressionRenderer: render %0 to %1", this.renderState, target);

        // Make a target div to hold our elements. We're going to set its inner HTML to the
        // LaTeX form that we got from the symbolic expression wrapper, and tell MathJax
        // that we want it to render as LaTeX.
        var elem = document.createElement("div");
        elem.innerHTML = "\\(" + this.renderState.source.Latex + "\\)";
        target.inlineTarget.appendChild(elem);

        if (MathJax) {
            console.log("SymbolicExpressionRenderer: queueing MathJax typeset...");
            MathJax.Hub.Queue(["Typeset", MathJax.Hub, elem]);
        }
    };

    return SymbolicExpressionRenderer;
})();

// Set up some config to hide messages from MathJax as it loads/typesets
var mathjaxConfig = document.createElement("script");
mathjaxConfig.type = "text/x-mathjax-config";
mathjaxConfig.innerText = "MathJax.Hub.Config({messageStyle: 'none'});";
document.getElementsByTagName("head")[0].appendChild(mathjaxConfig);

// Actually load up MathJax
var script = document.createElement("script");
script.type = "text/javascript";
script.src  = "https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.2/MathJax.js?config=TeX-MML-AM_CHTML";
document.getElementsByTagName("head")[0].appendChild(script);

// Register our simple renderer.
xamarin.interactive.RendererRegistry.registerRenderer (function(source) {
    if (source.$type === "SymbolicsRenderer.SymbolicExpressionWrapper")
        return new SymbolicExpressionRenderer;
});