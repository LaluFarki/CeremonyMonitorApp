/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./Views/**/*.cshtml",
              "./Pages/**/*.cshtml",
              "./wwwroot/**/*.js"
             ],
  theme: {
    extend: {
      colors: {
        navy: '#17233F',
        'blue-light': '#E7EEFC',
      },
    },
  },
  plugins: [],
}