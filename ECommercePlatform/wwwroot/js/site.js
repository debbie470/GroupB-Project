<script>

    document.addEventListener("DOMContentLoaded", function () {    // Wait for the full HTML document to be loaded and parsed

        // Assign function to the global window object so it can be called from HTML onclick events
        window.toggleSettings = function () {

            // Locate the settings panel element by its ID
            const panel = document.getElementById("settingsPanel");

            // Safety check: if the element doesn't exist, log a message and exit the function
            if (!panel) {
                console.log("Settings panel not found");
                return;
            }

            // If the panel is currently visible, hide it
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } 
            // If the panel is hidden, make it visible
            else {
                panel.style.display = "block";
            }
        };
    });
</script>