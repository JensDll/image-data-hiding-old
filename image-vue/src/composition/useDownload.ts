export function useDownload(uri: string, fileName: string) {
  const link = document.createElement('a');
  link.href = uri;
  link.download = fileName;
  link.click();
}
