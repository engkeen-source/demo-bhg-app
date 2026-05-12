import type { Config } from 'tailwindcss'

const config: Config = {
  content: ['./src/**/*.{js,ts,jsx,tsx,mdx}'],
  theme: {
    extend: {
      colors: {
        boss: {
          beige1:  '#E7D6C5',
          beige2:  '#F3EAE2',
          dark:    '#404040',
          brown:   '#6C4C2C',
          brownHover: '#553A20',
          white:   '#FFFFFF',
          content: '#FAF8F5',
          card:    '#FFFFFF',
          border:  '#E5DDD3',
          inputBorder: '#D8CFC4',
          divider: '#C8B4A0',
        },
      },
      fontFamily: {
        calibri: [
          'Calibri',
          'Trebuchet MS',
          'Liberation Sans',
          'Arial',
          'sans-serif',
        ],
      },
      fontSize: {
        'calibri-9':   ['9pt',  { lineHeight: '1.2' }],
        'calibri-10':  ['10pt', { lineHeight: '1.4' }],
        'calibri-11':  ['11pt', { lineHeight: '1.4' }],
        'calibri-16':  ['15.75pt', { lineHeight: '1.2' }],
      },
    },
  },
  plugins: [],
}

export default config
