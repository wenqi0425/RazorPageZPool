// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



function redirect(id) {
  window.location = `/Rides/RideDetails/${id}`;
}

function redirectToWhoBookedTheRide(id) {
    window.location = `/Identity/Account/Manage/WhoBookTheRide?id=${id}`;
}






