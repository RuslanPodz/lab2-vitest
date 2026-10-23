# Лабораторна робота № 2 — Vitest (варіант 11)

Модульне тестування модуля розрахунку замовлення `src/order.ts` та функції
`convertCurrency(amount, rate, fee)` за варіантом 11.

## Запуск

```bash
npm install
npm test            # прогін тестів
npm run coverage    # прогін із покриттям; звіт — coverage/index.html
```

## Структура

- `src/order.ts` — модуль із методички + `convertCurrency` (варіант 11)
- `tests/order.test.ts` — тести частини 1 (показовий приклад, доповнений)
- `tests/convertCurrency.test.ts` — тести функції за варіантом
- `vitest.config.ts` — конфігурація з порогами покриття 90 %
