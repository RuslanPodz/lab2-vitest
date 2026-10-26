import { describe, it, expect } from 'vitest';
import { convertCurrency } from '../src/order';

describe('convertCurrency — коректна конвертація', () => {
  it.each([
    { amount: 100, rate: 41.25, fee: 0, expected: 4125 },
    { amount: 100, rate: 41.25, fee: 0.01, expected: 4083.75 },
    { amount: 1000, rate: 41.5, fee: 0.05, expected: 39425 },
    { amount: 250, rate: 0.0243, fee: 0.02, expected: 5.95 },
    { amount: 12.5, rate: 1.08, fee: 0.015, expected: 13.3 },
  ])(
    '$amount × курс $rate з комісією $fee → $expected',
    ({ amount, rate, fee, expected }) => {
      expect(convertCurrency(amount, rate, fee)).toBeCloseTo(expected, 2);
    },
  );

  it('повертає 0 для нульової суми', () => {
    expect(convertCurrency(0, 41.25, 0.01)).toBe(0);
  });
});

describe('convertCurrency — заокруглення до двох знаків', () => {
  it.each([
    { rate: 1.234, expected: 1.23 },
    { rate: 1.236, expected: 1.24 },
    { rate: 1.125, expected: 1.13 },
  ])(
    '1 одиниця за курсом $rate заокруглюється до $expected',
    ({ rate, expected }) => {
      expect(convertCurrency(1, rate, 0)).toBe(expected);
    },
  );
});

describe('convertCurrency — межові значення', () => {
  it.each([
    { amount: 0, expected: 0 },
    { amount: 0.01, expected: 0.4 },
  ])('сума $amount за курсом 40 → $expected', ({ amount, expected }) => {
    expect(convertCurrency(amount, 40, 0)).toBeCloseTo(expected, 2);
  });

  it('сума −0.01 (межа мінус один) відхиляється', () => {
    expect(() => convertCurrency(-0.01, 40, 0)).toThrow('INVALID_AMOUNT');
  });

  it.each([-0.01, 0])('курс %s відхиляється', (rate) => {
    expect(() => convertCurrency(100, rate, 0)).toThrow('INVALID_RATE');
  });

  it('мінімальний додатний курс 0.01 приймається', () => {
    expect(convertCurrency(100, 0.01, 0)).toBeCloseTo(1, 2);
  });

  it.each([
    { fee: 0, expected: 1000 },
    { fee: 0.01, expected: 990 },
    { fee: 0.04, expected: 960 },
    { fee: 0.05, expected: 950 },
  ])(
    'допустима комісія $fee: 100 × 10 → $expected',
    ({ fee, expected }) => {
      expect(convertCurrency(100, 10, fee)).toBeCloseTo(expected, 2);
    },
  );

  it.each([-0.01, 0.06])('комісія %s поза діапазоном відхиляється', (fee) => {
    expect(() => convertCurrency(100, 10, fee)).toThrow('INVALID_FEE');
  });
});

describe('convertCurrency — некоректні вхідні дані', () => {
  it.each([NaN, Infinity, -100])('відхиляє суму %s', (amount) => {
    expect(() => convertCurrency(amount, 40, 0)).toThrow('INVALID_AMOUNT');
  });

  it.each([NaN, Infinity, -41.25])('відхиляє курс %s', (rate) => {
    expect(() => convertCurrency(100, rate, 0)).toThrow('INVALID_RATE');
  });

  it.each([NaN, -Infinity, 1])('відхиляє комісію %s', (fee) => {
    expect(() => convertCurrency(100, 40, fee)).toThrow('INVALID_FEE');
  });

  it('спершу перевіряє суму, якщо некоректні всі аргументи', () => {
    expect(() => convertCurrency(-1, -1, -1)).toThrow('INVALID_AMOUNT');
  });
});
