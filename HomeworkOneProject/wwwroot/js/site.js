// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Hunter Winger - Contains simple script to assign team names on button press from animals.txt

$(document).ready(function() {
    // Create animalNames array to hold our team names from animals.txt
    let animalNames = [];

    $.get("../Text/animals.txt", function(data) {
        animalNames = data.split("\n").filter(elem => elem.trim() !== ""); // Store lines that aren't blank into our animalNames array
    });

    // Select all card elements
    const cards = document.querySelectorAll('.card-title');
    
    // Get the number of cards
    const numberOfCards = cards.length;
    
    console.log('Number of cards:', numberOfCards);

    $('#teamsBtn').click(function() {
        if (animalNames.length > 0) {

            for (let i = 0; i < numberOfCards; i++)
            {
                const cardId = 'cardTitle-' + i; // Create the id of the title we're looking to change, this is dynamically assigned when we create the cards in our .cshtml
                const cardElement = $('#' + cardId); // use the id to select the element we are looking for 
                const randomIndex = Math.floor(Math.random() * animalNames.length); // Get a random number to select from the list of animal names
                const randomAnimal = animalNames[randomIndex]; // Use that random number to select a name from the array
                cardElement.text(randomAnimal); // Change the text of the element to be the random animal name we just selected
            }
        } else {
            console.log('No animal names available.');
        }
    });
});
