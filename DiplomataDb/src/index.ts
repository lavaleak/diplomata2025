import * as client from '@prisma/client';
export * from '@prisma/client';

let PRISMA_CLIENT: client.PrismaClient | null = null;

const getDb = () => {
  if (PRISMA_CLIENT !== null) {
    return PRISMA_CLIENT;
  }
  PRISMA_CLIENT = new client.PrismaClient({
    log: ['warn', 'error'],
  });
  if (process.env.STAGE === 'Local' || process.env.STAGE === 'Test') {
    // eslint-disable-next-line @typescript-eslint/no-unused-expressions
    PRISMA_CLIENT.$queryRaw`PRAGMA journal_mode = WAL;`;
  }
  return PRISMA_CLIENT;
};

const prisma = getDb();

export { prisma };
