module.exports = {
    extends: ['@commitlint/config-conventional'],
    rules: {
        'header-min-length': [2, 'always', 10],
        'header-max-length': [2, 'always', 72],

        'body-max-line-length': [2, 'always', 72],
        'footer-max-line-length': [2, 'always', 72],

        'type-enum': [
            2,
            'always',
            [
                'feat',
                'fix',
                'docs',
                'style',
                'refactor',
                'perf',
                'test',
                'build',
                'ci',
                'chore',
                'revert',
            ],
        ],
    },
};
