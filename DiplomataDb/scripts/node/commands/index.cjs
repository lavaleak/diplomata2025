const database = require('./database.cjs');
const localEnvSetup = require('./localEnvSetup.cjs');

module.exports = {
  ...database,
  ...localEnvSetup,
};
