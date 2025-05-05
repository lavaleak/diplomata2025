import { prisma } from '../../index';

async function createExample() {
    const name = 'Diplomata Prisma Example';

    await prisma.example.upsert({
        where: {
            name,
        },
        update: {
            name,
        },
        create: {
            name,
        },
    });
}

export {
    createExample,
};