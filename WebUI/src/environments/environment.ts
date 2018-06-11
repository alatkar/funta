// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  firebase: {
    apiKey: "AIzaSyAU909lkmqD9cATnQPJq6MhlJjCa0pt6Ag",
    authDomain: "funtadb.firebaseapp.com",
    databaseURL: "https://funtadb.firebaseio.com",
    projectId: "funtadb",
    storageBucket: "funtadb.appspot.com",
    messagingSenderId: "484352277159"
  }
};
