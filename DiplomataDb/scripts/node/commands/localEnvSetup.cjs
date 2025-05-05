/* eslint-disable security/detect-non-literal-fs-filename */
const fs = require('node:fs');
const path = require('node:path');
const dotenv = require('dotenv');
const { PATHS, getEnvVar } = require('../utils.cjs');

const generateEnvFiles = {
  label: 'Generating the .env file for the current stage.',
  task: (next) => {
    const stage = getEnvVar('STAGE', 'local');
    const exampleEnvFile = path.resolve(PATHS.app.configDir, '.env.example');
    const stageEnvFile = path.resolve(PATHS.app.configDir, `.env.${stage}`);
    const rootEnvFile = path.resolve(PATHS.app.root, '.env');

    if (!fs.existsSync(stageEnvFile) && stage === 'local') {
      console.log('Creating the .env.local from .env.example...');
      fs.copyFileSync(exampleEnvFile, stageEnvFile);
    }

    console.log(`cp -v ./config/.env.${stage} .env`);
    fs.copyFileSync(stageEnvFile, rootEnvFile);
    dotenv.config();

    process.env.ESLINT_USE_FLAT_CONFIG = true;

    next();
  },
};

module.exports = {
  generateEnvFiles,
};
