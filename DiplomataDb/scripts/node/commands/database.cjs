/* eslint-disable security/detect-non-literal-fs-filename */
const fs = require('node:fs');
const path = require('node:path');
const rimraf = require('rimraf');
const { cmdTask } = require('scrapty');
const { getPrismaSchemaPath, watchCmd } = require('../utils.cjs');

const generatePrismaSqliteDbFileFunc = () => {
  console.log('Initiating the generation of the Prisma local file from the cloud file...');
  const cloudSchemaPath = path.resolve(`./prisma/core.mysql.prisma`);
  const localSchemaPath = path.resolve(`./prisma/core.sqlite.prisma`);

  const fileContents = `
// ---------------------------------------------------------

// DON'T EDIT THIS FILE!

// It was generated from script and is always replaced.
// Edit the main file instead: ./prisma/core.mysql.prisma

// ---------------------------------------------------------
` + fs.readFileSync(cloudSchemaPath, 'utf8')
      .replace(/@db\.(.+?)\)/gm, '')
      .replace(/(@db\.Text)/gm, '')
      .replace(/(@db\.Double)/gm, '')
      .replace(/(@db\.TinyInt)/gm, '')
      .replace(/(@db\.UnsignedTinyInt)/gm, '')
      .replace('provider = "mysql"', 'provider = "sqlite"')
      .replace('provider = "postgresql"', 'provider = "sqlite"')
      .replace(/[ ]+$/gm, '');

  rimraf.sync(localSchemaPath);
  fs.writeFileSync(localSchemaPath, fileContents);
  console.log('Prisma local file was generated with success!');
};

const generatePrismaSqliteDbFile = {
  label: 'Generating the prisma local file from the cloud file.',
  task: (next) => {
    generatePrismaSqliteDbFileFunc();
    next();
  },
};

const generatePrismaSqliteDbFileWatch = {
  label: 'Generating the prisma local file from the cloud file in watch mode.',
  task: async (next) => {
    watchCmd(
      [
        path.resolve(process.cwd(), 'prisma/schema_cloud_core.prisma'),
      ],
      () => {
        generatePrismaSqliteDbFileFunc();
        // TODO: To execute the bootstrap you need to disconnect from the server.
        // const schemaFile = getPrismaSchemaPath();
        // execCmd(
        //   `prisma generate --schema ${schemaFile} && ` +
        //   `prisma db push --schema ${schemaFile} && ` +
        //   `prisma db seed --schema ${schemaFile}`
        // );
      },
      { executeOnStart: false },
    );
    next();
  },
};

const dbBootstrap = {
  label: 'Executing the database bootstrap using Prisma.',
  task: (next) => {
    const schemaFile = getPrismaSchemaPath();
    cmdTask(next,
      `prisma generate --schema ${schemaFile} && ` +
      `prisma db push --schema ${schemaFile} && ` +
      `prisma db seed --schema ${schemaFile}`);
  },
};

module.exports = {
  generatePrismaSqliteDbFile,
  generatePrismaSqliteDbFileWatch,
  dbBootstrap,
};
