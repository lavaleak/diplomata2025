const { runQueue } = require('scrapty');
const scripts = require('./runScript.cjs');

const exec = () => {
  const result = {
    cmd: null,
    error: null,
  };

  if (process.argv?.length <= 2) {
    result.error = new Error(`\x1b[0;31m
      No command was passed!
  \x1b[0m`);
  }

  result.cmd = process.argv[2];

  if (!Object.keys(scripts).includes(result.cmd)) {
    result.error = new Error(`\x1b[0;31m
      The command "${result.cmd}" is invalid!
  \x1b[0m`);
    result.cmd = 'help';
  }

  runQueue(scripts[result.cmd].queue);

  if (result.error) {
    throw result.error;
  };
};

exec();
