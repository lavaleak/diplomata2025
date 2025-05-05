const {
  generateEnvFiles,
  generatePrismaSqliteDbFile,
  dbBootstrap,
} = require('./commands/index.cjs');

const scripts = {
  bootstrap: {
    queue: [
      generateEnvFiles,
      generatePrismaSqliteDbFile,
      dbBootstrap,
    ],
    description: 'Execute the necessary to configure the database before init the server.',
  },
  help: {
    queue: [],
    description: 'Show the Node.js available commands.',
  },
};

scripts.help.queue.push({
  label: 'Showing the available commands.',
  task: (next) => {
    console.log(`
  pnpm scripts <command>

COMMANDS:

    ${Object.keys(scripts).map(key => `\x1b[0;36m${key}\x1b[0m - ${scripts[key].description}`).join('\n    ')}
`);
    next();
  },
});

module.exports = scripts;
