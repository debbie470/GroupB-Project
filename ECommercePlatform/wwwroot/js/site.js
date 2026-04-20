<script>
    document.addEventListener("DOMContentLoaded", function () {

        window.toggleSettings = function () {
            const panel = document.getElementById("settingsPanel");

            if (!panel) {
                console.log("Settings panel not found");
                return;
            }

            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
        };

});
</script>

