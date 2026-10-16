# Лабораторна робота № 1

**Розгортання середовища автоматизації тестування. Структура проєкту та перший автоматизований тест**

Виконавець: Подзивалов Руслан Олександрович, група ІПЗ 2 курс магістр

Варіант 11: перетворення числа на римський запис (від 1 до 3999), відхилення значень поза діапазоном.

## Структура проєкту

```
qa-automation/
├── src/
│   ├── temperature.ts
│   └── roman.ts
├── tests/
│   ├── temperature.test.ts
│   └── roman.test.ts
├── .gitignore
├── .prettierrc
├── eslint.config.mjs
├── package.json
├── tsconfig.json
├── vitest.config.ts
└── README.md
```

- `src/` — код, який тестується
- `tests/` — тести Vitest
- `vitest.config.ts` — налаштування раннера тестів і покриття
- `eslint.config.mjs`, `.prettierrc` — лінтер і форматування
- `tsconfig.json` — налаштування TypeScript

## Встановлення

```
npm install
```

## Запуск

```
npm test
npm run test:watch
npm run coverage
npm run lint
npm run format
```

Звіт про покриття відкривається з файлу `coverage/index.html`.
