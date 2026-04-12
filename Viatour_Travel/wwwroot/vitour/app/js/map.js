(function () {
    const MAPBOX_TOKEN = "YOUR_MAPBOX_PUBLIC_TOKEN";

    const defaultCenter = [-0.108968, 51.492933];
    const defaultZoom = 14;

    function createMap(containerId, coordinates) {
        const container = document.getElementById(containerId);

        if (!container) {
            return;
        }

        mapboxgl.accessToken = MAPBOX_TOKEN;

        const map = new mapboxgl.Map({
            container: containerId,
            style: "mapbox://styles/mapbox/light-v11",
            center: coordinates,
            zoom: defaultZoom
        });

        const markerElement = document.createElement("div");
        markerElement.className = "marker";

        new mapboxgl.Marker(markerElement)
            .setLngLat(coordinates)
            .addTo(map);
    }

    createMap("map", defaultCenter);
    createMap("map2", defaultCenter);
    createMap("map3", defaultCenter);
})();