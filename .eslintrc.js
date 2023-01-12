module.exports = {
  env: {
    browser: true,
    es2021: true,
    jest: true,
    node: true
  },
  extends: ['eslint:recommended'],
  plugins: ['react', '@typescript-eslint', 'jsonc', 'jsx-a11y', 'prettier', 'typescript-sort-keys'],
  ignorePatterns: ['!.*', 'dist', 'node_modules'],
  rules: {
    'prettier/prettier': 'error'
  },
  overrides: [
    {
      files: ['*.js'],
      globals: { __DEV__: true },
      extends: ['eslint:recommended', 'prettier', 'plugin:prettier/recommended'],
      parser: '@typescript-eslint/parser',
      parserOptions: {
        ecmaFeatures: {
          jsx: true
        },
        ecmaVersion: 'latest',
        sourceType: 'module'
      },
      rules: {
        '@typescript/no-var-requires': 'off'
      }
    },
    {
      files: ['*.ts', '*.tsx'],
      parser: '@typescript-eslint/parser',
      globals: { __DEV__: true },
      parserOptions: {
        ecmaFeatures: {
          jsx: true
        },
        ecmaVersion: 'latest',
        sourceType: 'module',
        project: './tsconfig.json'
      },
      extends: [
        'eslint:recommended',
        'plugin:react/recommended',
        'plugin:@typescript-eslint/recommended',
        'plugin:jsx-a11y/recommended',
        'plugin:sonarjs/recommended',
        'prettier',
        'plugin:prettier/recommended'
      ],
      settings: {
        react: {
          version: 'detect'
        }
      },
      rules: {
        'sonarjs/no-duplicate-string': 'off',
        '@typescript-eslint/no-unused-vars': 'error',
        'react/react-in-jsx-scope': 'off',
        'typescript-sort-keys/interface': [
          'error',
          'asc',
          { 'caseSensitive': false, 'natural': true, 'requiredFirst': true }
        ]
      }
    },
    {
      files: ['*.json'],
      extends: ['plugin:jsonc/recommended-with-json', 'plugin:jsonc/prettier'],
      parser: 'jsonc-eslint-parser',
      parserOptions: {
        jsonSyntax: 'JSON'
      }
    },
    {
      files: ['*.jsonc'],
      extends: ['plugin:jsonc/recommended-with-jsonc'],
      parser: 'jsonc-eslint-parser',
      parserOptions: {
        jsonSyntax: 'JSONC'
      }
    },
    {
      files: ['*.json5'],
      extends: ['plugin:jsonc/recommended-with-json5'],
      parser: 'jsonc-eslint-parser',
      parserOptions: {
        jsonSyntax: 'JSON5'
      }
    }
  ]
};
