const path = require('node:path');
const net = require('node:net');
const chokidar = require('chokidar');
const { execCmd } = require('scrapty');
const colors = require('colors/safe');

function getPaths() {
  const paths = {
    app: {
      root: path.resolve(process.cwd()),
      binDir: '',
      configDir: '',
      config: {},
      bin: {},
    },
  };

  paths.app.binDir = path.resolve(paths.app.root, 'node_modules', '.bin');
  paths.app.configDir = path.resolve(paths.app.root, 'config');

  paths.app.bin = {
    prisma: path.resolve(paths.app.binDir, 'prisma'),
    concurrently: path.resolve(paths.app.binDir, 'concurrently'),
  };

  return paths;
}

function getEnvVar(envKey, fallback = null) {
  if (!Object.keys(process.env).includes(envKey) && !fallback) {
    throw new Error(`The environment variable "${envKey}" was not defined!`);
  }
  return process.env[envKey] ?? fallback;
}

const getPrismaSchemaPath = () => {
  const isLocalOrStage = () => {
    console.log(`STAGE: ${process.env.STAGE}`);
    return process.env.STAGE && (process.env.STAGE === 'local' || process.env.STAGE === 'test');
  };

  const localPrismaPath = `./prisma/core.sqlite.prisma`;
  const cloudPrismaPath = `./prisma/core.mysql.prisma`;

  return isLocalOrStage() ? localPrismaPath : cloudPrismaPath;
};

const checkPort = (port, host = '127.0.0.1') => {
  function getDeferred() {
    let resolve, reject, promise = new Promise(function (res, rej) {
      resolve = res;
      reject = rej;
    });

    return {
      resolve: resolve,
      reject: reject,
      promise: promise,
    };
  }

  const deferred = getDeferred();
  const client = new net.Socket();

  function cleanUp() {
    client.removeAllListeners('connect');
    client.removeAllListeners('error');
    client.end();
    client.destroy();
    client.unref();
  }

  client.once('connect', () => {
    deferred.resolve(true);
    cleanUp();
  });

  client.once('error', (err) => {
    if (err.code !== 'ECONNREFUSED') {
      deferred.reject(err);
    } else {
      deferred.resolve(false);
    }
    cleanUp();
  });

  client.connect({
    port,
    host,
  });

  return deferred.promise;
};

const watchCmd = (paths, cmd, options = { executeOnStart: true }) => {
  const watcher = chokidar.watch(paths, { persistent: true });
  let isRunning = false;
  let executedOnStart = false;

  watcher.on('all', async (event, path) => {
    if (isRunning) return;
    if ((event === 'add' || event === 'addDir') && !options?.executeOnStart && !executedOnStart) return;

    executedOnStart = true;
    isRunning = true;

    let logMsg = `\nEvent ${colors.blue.italic(event)} detected in ${colors.blue.italic(path)}\n`;

    switch (typeof cmd) {
      case 'string':
        logMsg += `${colors.bold(`Executing command: ${colors.green(cmd)}`)}\n`;
        console.log(logMsg);
        await execCmd(cmd);
        break;
      case 'function':
        logMsg += `${colors.bold('Executing corresponding action...')}\n`;
        console.log(logMsg);
        await cmd();
        break;
      default:
        logMsg += `${colors.bold('No action to execute.')}\n`;
        console.log(logMsg);
    }

    isRunning = false;
  });
};

const validatePort = async (serviceName, port) => {
  const portInUse = await checkPort(port);

  if (portInUse) {
      throw new Error(`Port ${port} of ${serviceName} already in use.`);
  }
};

const serviceLabel = (
  name,
  port,
  { host, protocol } = {
    host: 'localhost',
    protocol: 'http',
  }) => console.log(`\nStarting the ${colors.green.bold(name)} in ${colors.blue.bold.underline(`${protocol}://${host}:${port}`)}\n`);

module.exports = {
  PATHS: getPaths(),
  getEnvVar,
  getPrismaSchemaPath,
  checkPort,
  watchCmd,
  validatePort,
  serviceLabel,
};
