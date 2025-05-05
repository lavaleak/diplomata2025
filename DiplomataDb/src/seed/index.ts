import * as core from './core';
import { prisma } from '../index';
import * as dotenv from 'dotenv';

async function executeSeed() {
  // ⚠️ The order matters. ⚠️
  await core.example.createExample();
}

const main = () => {
  dotenv.config();
  executeSeed()
    .then(async () => {
      await prisma.$disconnect();
    })
    .catch(async (e) => {
      console.error(e);
      await prisma.$disconnect();
      process.exit(1);
    });
};

main();
