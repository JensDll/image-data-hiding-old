export function useDownload() {
  const createAndDownload = (file: Blob, filename: string) => {
    const a = document.createElement('a');

    a.href = URL.createObjectURL(file);
    a.download = filename;
    a.click();

    URL.revokeObjectURL(a.href);
  };

  return {
    saveImage(file: Blob, filename: string) {
      createAndDownload(file, filename);
    },
    saveTextFile(content: string, filename: string, contentType: string) {
      const file = new Blob([content], { type: contentType });
      createAndDownload(file, filename);
    }
  };
}
